// Firebase Messaging Service Worker for Jadwa API
// This file should be placed in the public folder of your frontend application
// File name must be exactly: firebase-messaging-sw.js

// Import Firebase scripts
importScripts('https://www.gstatic.com/firebasejs/10.7.1/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/10.7.1/firebase-messaging-compat.js');

// Firebase configuration (same as in your main app)
const firebaseConfig = {
  apiKey: "your-api-key",
  authDomain: "jadwa-api-project.firebaseapp.com",
  projectId: "jadwa-api-project",
  storageBucket: "jadwa-api-project.appspot.com",
  messagingSenderId: "123456789",
  appId: "1:123456789:web:abcdef123456"
};

// Initialize Firebase
firebase.initializeApp(firebaseConfig);

// Retrieve Firebase Messaging object
const messaging = firebase.messaging();

// Handle background messages
messaging.onBackgroundMessage(function(payload) {
  console.log('[firebase-messaging-sw.js] Received background message:', payload);

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

  // Show the notification
  return self.registration.showNotification(notificationTitle, notificationOptions);
});

// Handle notification click events
self.addEventListener('notificationclick', function(event) {
  console.log('[firebase-messaging-sw.js] Notification click received:', event);

  event.notification.close();

  const action = event.action;
  const data = event.notification.data;

  if (action === 'dismiss') {
    // Just close the notification
    return;
  }

  // Handle view action or notification click
  let urlToOpen = '/';

  if (data) {
    // Determine URL based on notification type and data
    switch (data.messageType) {
      case 'MSG002':
      case 'MSG003':
      case 'MSG004':
      case 'MSG005':
      case 'MSG006':
        // Fund-related notifications - navigate to fund details
        if (data.fundId) {
          urlToOpen = `/funds/${data.fundId}`;
        } else {
          urlToOpen = '/funds';
        }
        break;
      default:
        // Default to dashboard or funds list
        urlToOpen = '/funds';
        break;
    }
  }

  // Open the URL in a new tab or focus existing tab
  event.waitUntil(
    clients.matchAll({ type: 'window', includeUncontrolled: true }).then(function(clientList) {
      // Check if there's already a window/tab open with the target URL
      for (let i = 0; i < clientList.length; i++) {
        const client = clientList[i];
        if (client.url.includes(urlToOpen) && 'focus' in client) {
          return client.focus();
        }
      }

      // If no existing window/tab, open a new one
      if (clients.openWindow) {
        return clients.openWindow(urlToOpen);
      }
    })
  );
});

// Handle notification close events
self.addEventListener('notificationclose', function(event) {
  console.log('[firebase-messaging-sw.js] Notification closed:', event);
  
  // Optional: Send analytics or tracking data
  const data = event.notification.data;
  if (data && data.messageType) {
    // You could send a tracking event here
    console.log(`Notification ${data.messageType} was closed without action`);
  }
});

// Handle push events (for additional processing)
self.addEventListener('push', function(event) {
  console.log('[firebase-messaging-sw.js] Push event received:', event);
  
  // Firebase messaging handles the push event automatically,
  // but you can add custom logic here if needed
});

// Service worker installation
self.addEventListener('install', function(event) {
  console.log('[firebase-messaging-sw.js] Service worker installing...');
  
  // Skip waiting to activate immediately
  self.skipWaiting();
});

// Service worker activation
self.addEventListener('activate', function(event) {
  console.log('[firebase-messaging-sw.js] Service worker activating...');
  
  // Claim all clients immediately
  event.waitUntil(self.clients.claim());
});

// Handle message events from the main thread
self.addEventListener('message', function(event) {
  console.log('[firebase-messaging-sw.js] Message received from main thread:', event.data);
  
  if (event.data && event.data.type === 'SKIP_WAITING') {
    self.skipWaiting();
  }
});

// Error handling
self.addEventListener('error', function(event) {
  console.error('[firebase-messaging-sw.js] Service worker error:', event.error);
});

// Unhandled promise rejection handling
self.addEventListener('unhandledrejection', function(event) {
  console.error('[firebase-messaging-sw.js] Unhandled promise rejection:', event.reason);
});

// Utility function to get notification icon based on message type
function getNotificationIcon(messageType) {
  switch (messageType) {
    case 'MSG002':
      return '/icons/fund-creation.png';
    case 'MSG003':
      return '/icons/user-removal.png';
    case 'MSG004':
      return '/icons/date-change.png';
    case 'MSG005':
      return '/icons/user-assignment.png';
    case 'MSG006':
      return '/icons/fund-update.png';
    default:
      return '/favicon.ico';
  }
}

// Utility function to get notification badge based on priority
function getNotificationBadge(priority) {
  switch (priority) {
    case 'High':
      return '/badges/high-priority.png';
    case 'Normal':
      return '/badges/normal-priority.png';
    case 'Low':
      return '/badges/low-priority.png';
    default:
      return '/badge-icon.png';
  }
}
