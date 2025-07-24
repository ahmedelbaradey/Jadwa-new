// Firebase configuration for Jadwa API push notifications
// This file should be included in the frontend application

// Import the functions you need from the SDKs you need
import { initializeApp } from 'firebase/app';
import { getMessaging, getToken, onMessage } from 'firebase/messaging';

// Your web app's Firebase configuration
// Replace with your actual Firebase project configuration
const firebaseConfig = {
  apiKey: "your-api-key",
  authDomain: "jadwa-api-project.firebaseapp.com",
  projectId: "jadwa-api-project",
  storageBucket: "jadwa-api-project.appspot.com",
  messagingSenderId: "123456789",
  appId: "1:123456789:web:abcdef123456"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

// Initialize Firebase Cloud Messaging and get a reference to the service
const messaging = getMessaging(app);

// Your VAPID key from Firebase Console
const vapidKey = "your-vapid-key";

/**
 * Requests permission for notifications and gets the FCM token
 * @returns {Promise<string|null>} FCM token or null if permission denied
 */
export async function requestNotificationPermission() {
  try {
    // Request permission for notifications
    const permission = await Notification.requestPermission();
    
    if (permission === 'granted') {
      console.log('Notification permission granted.');
      
      // Get the FCM token
      const token = await getToken(messaging, { vapidKey });
      
      if (token) {
        console.log('FCM Token:', token);
        return token;
      } else {
        console.log('No registration token available.');
        return null;
      }
    } else {
      console.log('Notification permission denied.');
      return null;
    }
  } catch (error) {
    console.error('Error getting notification permission:', error);
    return null;
  }
}

/**
 * Registers the FCM token with the Jadwa API backend
 * @param {string} token - FCM token to register
 * @param {string} apiBaseUrl - Base URL of the Jadwa API
 * @param {string} authToken - JWT authentication token
 * @returns {Promise<boolean>} Success status
 */
export async function registerTokenWithBackend(token, apiBaseUrl, authToken) {
  try {
    const response = await fetch(`${apiBaseUrl}/api/DeviceToken/Register`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${authToken}`
      },
      body: JSON.stringify({
        token: token,
        deviceType: 1, // Web = 1
        userAgent: navigator.userAgent,
        metadata: JSON.stringify({
          browser: getBrowserInfo(),
          timestamp: new Date().toISOString()
        })
      })
    });

    if (response.ok) {
      const result = await response.json();
      console.log('Token registered successfully:', result);
      return true;
    } else {
      console.error('Failed to register token:', response.statusText);
      return false;
    }
  } catch (error) {
    console.error('Error registering token:', error);
    return false;
  }
}

/**
 * Sets up foreground message handling
 * @param {function} onMessageCallback - Callback function to handle incoming messages
 */
export function setupForegroundMessageHandling(onMessageCallback) {
  onMessage(messaging, (payload) => {
    console.log('Message received in foreground:', payload);
    
    // Extract notification data
    const notificationTitle = payload.notification?.title || 'Jadwa Notification';
    const notificationOptions = {
      body: payload.notification?.body || 'You have a new notification',
      icon: '/favicon.ico',
      badge: '/badge-icon.png',
      tag: payload.data?.messageType || 'general',
      data: payload.data,
      requireInteraction: true,
      actions: [
        {
          action: 'view',
          title: 'View',
          icon: '/view-icon.png'
        },
        {
          action: 'dismiss',
          title: 'Dismiss',
          icon: '/dismiss-icon.png'
        }
      ]
    };

    // Show notification if page is not in focus
    if (document.hidden) {
      new Notification(notificationTitle, notificationOptions);
    }

    // Call the provided callback
    if (onMessageCallback) {
      onMessageCallback(payload);
    }
  });
}

/**
 * Initializes Firebase messaging for the application
 * @param {string} apiBaseUrl - Base URL of the Jadwa API
 * @param {string} authToken - JWT authentication token
 * @param {function} onMessageCallback - Callback for handling messages
 * @returns {Promise<boolean>} Success status
 */
export async function initializeFirebaseMessaging(apiBaseUrl, authToken, onMessageCallback) {
  try {
    // Check if the browser supports notifications
    if (!('Notification' in window)) {
      console.warn('This browser does not support notifications');
      return false;
    }

    // Check if service workers are supported
    if (!('serviceWorker' in navigator)) {
      console.warn('This browser does not support service workers');
      return false;
    }

    // Register service worker
    await registerServiceWorker();

    // Request permission and get token
    const token = await requestNotificationPermission();
    
    if (token) {
      // Register token with backend
      const registered = await registerTokenWithBackend(token, apiBaseUrl, authToken);
      
      if (registered) {
        // Setup foreground message handling
        setupForegroundMessageHandling(onMessageCallback);
        
        console.log('Firebase messaging initialized successfully');
        return true;
      }
    }

    return false;
  } catch (error) {
    console.error('Error initializing Firebase messaging:', error);
    return false;
  }
}

/**
 * Registers the service worker for background notifications
 */
async function registerServiceWorker() {
  try {
    const registration = await navigator.serviceWorker.register('/firebase-messaging-sw.js');
    console.log('Service Worker registered:', registration);
    return registration;
  } catch (error) {
    console.error('Service Worker registration failed:', error);
    throw error;
  }
}

/**
 * Gets browser information for metadata
 * @returns {object} Browser information
 */
function getBrowserInfo() {
  const ua = navigator.userAgent;
  let browser = 'Unknown';
  
  if (ua.includes('Chrome')) browser = 'Chrome';
  else if (ua.includes('Firefox')) browser = 'Firefox';
  else if (ua.includes('Safari')) browser = 'Safari';
  else if (ua.includes('Edge')) browser = 'Edge';
  
  return {
    name: browser,
    userAgent: ua,
    language: navigator.language,
    platform: navigator.platform
  };
}

/**
 * Deactivates the current device token
 * @param {string} apiBaseUrl - Base URL of the Jadwa API
 * @param {string} authToken - JWT authentication token
 * @returns {Promise<boolean>} Success status
 */
export async function deactivateToken(apiBaseUrl, authToken) {
  try {
    const token = await getToken(messaging, { vapidKey });
    
    if (token) {
      const response = await fetch(`${apiBaseUrl}/api/DeviceToken/Deactivate`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${authToken}`
        },
        body: JSON.stringify(token)
      });

      if (response.ok) {
        console.log('Token deactivated successfully');
        return true;
      }
    }

    return false;
  } catch (error) {
    console.error('Error deactivating token:', error);
    return false;
  }
}

export { messaging };
