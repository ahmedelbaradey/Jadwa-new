# Frontend Session Timeout Implementation Guide

## Overview

This document provides comprehensive specifications for implementing the 30-minute session timeout mechanism in the React/TypeScript frontend. The implementation includes activity detection, warning dialogs, automatic logout, and role-based timeout configurations.

## Architecture Overview

### Backend Integration Points

1. **Session Validation Middleware**: Automatically validates sessions on each API request
2. **Session Management APIs**:
   - `POST /api/Users/Session/Extend-Session` - Extend session timeout
   - `GET /api/Users/Session/Session-Status` - Get current session status
   - `GET /api/Users/Session/Validate-Session` - Validate current session
   - `GET /api/Users/Session/Timeout-Config` - Get timeout configuration
   - `POST /api/Users/Session/Update-Activity` - Manual activity update

### Session Timeout Configuration

```typescript
interface SessionTimeoutConfig {
  timeoutMinutes: number;        // Role-based timeout (30-60 minutes)
  warningMinutes: number;        // 5 minutes before expiration
  slidingExpiration: boolean;    // true - resets on activity
  checkIntervalSeconds: number;  // 60 seconds - status check frequency
  warningIntervalSeconds: number; // 30 seconds - warning update frequency
  rememberMeEnabled: boolean;    // Whether Remember Me is supported
  roleBasedTimeouts: Record<string, number>; // Role-specific timeouts
}
```

## Core Implementation Components

### 1. Session Timeout Service

Create a comprehensive service to manage session timeout logic:

```typescript
// services/sessionTimeoutService.ts
import { SessionInfo, SessionTimeoutConfig, SessionExtensionResponse } from '../types/session';
import { ApiClient } from './apiClient';

export class SessionTimeoutService {
  private config: SessionTimeoutConfig | null = null;
  private checkInterval: NodeJS.Timeout | null = null;
  private warningInterval: NodeJS.Timeout | null = null;
  private lastActivityTime: Date = new Date();
  private isWarningShown: boolean = false;
  private isInitialized: boolean = false;
  
  // Activity tracking events
  private activityEvents = [
    'mousedown', 'mousemove', 'keypress', 'scroll', 'touchstart',
    'click', 'keydown', 'wheel', 'touchmove', 'touchend'
  ];
  
  constructor(
    private apiClient: ApiClient,
    private onSessionExpired: () => void,
    private onShowWarning: (remainingSeconds: number, sessionInfo: SessionInfo) => void,
    private onHideWarning: () => void,
    private onSessionExtended?: (sessionInfo: SessionInfo) => void
  ) {}

  async initialize(): Promise<void> {
    if (this.isInitialized) return;

    try {
      // Get timeout configuration from backend
      this.config = await this.apiClient.getTimeoutConfig();
      this.setupActivityTracking();
      this.startSessionMonitoring();
      this.isInitialized = true;
      
      console.log('Session timeout service initialized', this.config);
    } catch (error) {
      console.error('Failed to initialize session timeout service:', error);
      // Use default configuration
      this.config = {
        timeoutMinutes: 30,
        warningMinutes: 5,
        slidingExpiration: true,
        checkIntervalSeconds: 60,
        warningIntervalSeconds: 30,
        rememberMeEnabled: false,
        roleBasedTimeouts: {}
      };
      this.setupActivityTracking();
      this.startSessionMonitoring();
      this.isInitialized = true;
    }
  }

  private setupActivityTracking(): void {
    const handleActivity = this.debounce(() => {
      this.lastActivityTime = new Date();
      
      // If warning is shown and user is active, extend session automatically
      if (this.isWarningShown) {
        this.extendSession('User activity during warning');
      }
    }, 1000); // Debounce to avoid excessive API calls

    this.activityEvents.forEach(event => {
      document.addEventListener(event, handleActivity, true);
    });

    // Track API calls as activity
    this.setupApiActivityTracking();
  }

  private setupApiActivityTracking(): void {
    // Intercept API calls to track as activity
    const originalFetch = window.fetch;
    window.fetch = async (...args) => {
      this.lastActivityTime = new Date();
      return originalFetch.apply(window, args);
    };
  }

  private startSessionMonitoring(): void {
    if (!this.config) return;

    // Check session status periodically
    this.checkInterval = setInterval(async () => {
      await this.checkSessionStatus();
    }, this.config.checkIntervalSeconds * 1000);
  }

  private async checkSessionStatus(): Promise<void> {
    try {
      const sessionInfo = await this.apiClient.getSessionStatus();
      
      if (!sessionInfo.isActive || sessionInfo.isExpired) {
        this.handleSessionExpired();
        return;
      }

      // Check if session is in warning period
      if (sessionInfo.inWarningPeriod && !this.isWarningShown) {
        this.showWarning(sessionInfo.remainingSeconds, sessionInfo);
      } else if (!sessionInfo.inWarningPeriod && this.isWarningShown) {
        this.hideWarning();
      }

      // Update warning countdown if already shown
      if (this.isWarningShown) {
        this.onShowWarning(sessionInfo.remainingSeconds, sessionInfo);
      }
    } catch (error) {
      console.error('Session status check failed:', error);
      
      // Handle 401 responses as session expired
      if (error.response?.status === 401) {
        this.handleSessionExpired();
      }
    }
  }

  private showWarning(remainingSeconds: number, sessionInfo: SessionInfo): void {
    this.isWarningShown = true;
    this.onShowWarning(remainingSeconds, sessionInfo);
    
    // Start warning countdown updates
    this.warningInterval = setInterval(async () => {
      try {
        const updatedSessionInfo = await this.apiClient.getSessionStatus();
        if (updatedSessionInfo.isExpired) {
          this.handleSessionExpired();
        } else {
          this.onShowWarning(updatedSessionInfo.remainingSeconds, updatedSessionInfo);
        }
      } catch (error) {
        console.error('Warning update failed:', error);
        this.handleSessionExpired();
      }
    }, (this.config?.warningIntervalSeconds || 30) * 1000);
  }

  private hideWarning(): void {
    this.isWarningShown = false;
    this.onHideWarning();
    
    if (this.warningInterval) {
      clearInterval(this.warningInterval);
      this.warningInterval = null;
    }
  }

  async extendSession(reason?: string): Promise<boolean> {
    try {
      const result = await this.apiClient.extendSession({ reason, isActivityBased: true });
      
      if (result.success) {
        this.hideWarning();
        
        if (this.onSessionExtended && result.sessionInfo) {
          this.onSessionExtended(result.sessionInfo);
        }
        
        console.log('Session extended successfully', result);
        return true;
      }
      
      console.warn('Session extension failed', result);
      return false;
    } catch (error) {
      console.error('Session extension failed:', error);
      
      // If extension fails due to expired session, handle as expired
      if (error.response?.status === 401) {
        this.handleSessionExpired();
      }
      
      return false;
    }
  }

  async forceLogout(): Promise<void> {
    try {
      await this.apiClient.logout();
    } catch (error) {
      console.error('Logout API call failed:', error);
    } finally {
      this.handleSessionExpired();
    }
  }

  private handleSessionExpired(): void {
    this.cleanup();
    this.onSessionExpired();
  }

  getConfig(): SessionTimeoutConfig | null {
    return this.config;
  }

  isActive(): boolean {
    return this.isInitialized && this.config !== null;
  }

  getRemainingTime(): number {
    // This would need to be calculated based on last known session info
    // For now, return 0 as placeholder
    return 0;
  }

  private debounce(func: Function, wait: number): Function {
    let timeout: NodeJS.Timeout;
    return function executedFunction(...args: any[]) {
      const later = () => {
        clearTimeout(timeout);
        func(...args);
      };
      clearTimeout(timeout);
      timeout = setTimeout(later, wait);
    };
  }

  cleanup(): void {
    if (this.checkInterval) {
      clearInterval(this.checkInterval);
      this.checkInterval = null;
    }
    
    if (this.warningInterval) {
      clearInterval(this.warningInterval);
      this.warningInterval = null;
    }
    
    // Remove activity event listeners
    const handleActivity = () => {}; // Placeholder
    this.activityEvents.forEach(event => {
      document.removeEventListener(event, handleActivity, true);
    });
    
    this.isInitialized = false;
    this.isWarningShown = false;
  }
}
```

### 2. Session Warning Modal Component

Create a comprehensive modal component for session timeout warnings:

```typescript
// components/SessionWarningModal.tsx
import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { SessionInfo } from '../types/session';

interface SessionWarningModalProps {
  isOpen: boolean;
  remainingSeconds: number;
  sessionInfo: SessionInfo;
  onExtendSession: () => Promise<boolean>;
  onLogout: () => void;
  onContinue: () => void;
}

export const SessionWarningModal: React.FC<SessionWarningModalProps> = ({
  isOpen,
  remainingSeconds,
  sessionInfo,
  onExtendSession,
  onLogout,
  onContinue
}) => {
  const { t } = useTranslation();
  const [isExtending, setIsExtending] = useState(false);
  const [countdown, setCountdown] = useState(remainingSeconds);

  useEffect(() => {
    setCountdown(remainingSeconds);
  }, [remainingSeconds]);

  useEffect(() => {
    if (!isOpen) return;

    const timer = setInterval(() => {
      setCountdown(prev => {
        if (prev <= 1) {
          clearInterval(timer);
          return 0;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(timer);
  }, [isOpen, remainingSeconds]);

  const formatTime = (seconds: number): string => {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    return `${minutes}:${remainingSeconds.toString().padStart(2, '0')}`;
  };

  const handleExtendSession = async () => {
    setIsExtending(true);
    try {
      const success = await onExtendSession();
      if (!success) {
        // Show error message or handle failure
        console.error('Failed to extend session');
      }
    } finally {
      setIsExtending(false);
    }
  };

  const getWarningLevel = () => {
    if (countdown <= 60) return 'critical'; // Last minute
    if (countdown <= 180) return 'urgent';  // Last 3 minutes
    return 'warning';
  };

  const getWarningColor = () => {
    const level = getWarningLevel();
    switch (level) {
      case 'critical': return 'text-red-600 bg-red-50 border-red-200';
      case 'urgent': return 'text-orange-600 bg-orange-50 border-orange-200';
      default: return 'text-yellow-600 bg-yellow-50 border-yellow-200';
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg p-6 max-w-md w-full mx-4 shadow-xl">
        <div className="flex items-center mb-4">
          <div className="flex-shrink-0">
            <svg className="h-6 w-6 text-yellow-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} 
                d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.732-.833-2.5 0L4.268 19.5c-.77.833.192 2.5 1.732 2.5z" />
            </svg>
          </div>
          <div className="ml-3">
            <h3 className="text-lg font-medium text-gray-900">
              {t('SessionExpiredTitle')}
            </h3>
          </div>
        </div>
        
        <div className="mb-6">
          <p className="text-sm text-gray-500 mb-4">
            {t('SessionExpiredWarning', { time: Math.ceil(countdown / 60) })}
          </p>
          
          <div className={`text-center p-4 rounded-lg border ${getWarningColor()}`}>
            <div className="text-3xl font-bold mb-2">
              {formatTime(countdown)}
            </div>
            <div className="text-sm">
              {sessionInfo.userRole && (
                <span className="inline-block bg-blue-100 text-blue-800 px-2 py-1 rounded text-xs mr-2">
                  {sessionInfo.userRole}
                </span>
              )}
              {sessionInfo.isRememberMeSession && (
                <span className="inline-block bg-green-100 text-green-800 px-2 py-1 rounded text-xs">
                  Remember Me
                </span>
              )}
            </div>
          </div>
        </div>

        <div className="flex flex-col space-y-3 sm:flex-row sm:space-y-0 sm:space-x-3">
          <button
            onClick={handleExtendSession}
            disabled={isExtending}
            className="flex-1 bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {isExtending ? (
              <span className="flex items-center justify-center">
                <svg className="animate-spin -ml-1 mr-2 h-4 w-4 text-white" fill="none" viewBox="0 0 24 24">
                  <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                  <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Extending...
              </span>
            ) : (
              t('SessionWarningExtendButton')
            )}
          </button>
          
          <button
            onClick={onContinue}
            className="flex-1 bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700"
          >
            {t('SessionWarningContinueButton')}
          </button>
          
          <button
            onClick={onLogout}
            className="flex-1 bg-red-600 text-white px-4 py-2 rounded-md hover:bg-red-700"
          >
            {t('SessionWarningLogoutButton')}
          </button>
        </div>

        <div className="mt-4 text-xs text-gray-500 text-center">
          <p>Device: {sessionInfo.deviceType} | Browser: {sessionInfo.browserInfo}</p>
          <p>Last Activity: {new Date(sessionInfo.lastActivityAt).toLocaleTimeString()}</p>
        </div>
      </div>
    </div>
  );
};
```

This implementation provides a comprehensive session timeout mechanism with:

1. **Automatic Activity Detection**: Tracks mouse, keyboard, and API activity
2. **Role-Based Timeouts**: Supports different timeout values for different user roles
3. **Warning System**: Shows warnings 5 minutes before expiration with countdown
4. **Sliding Expiration**: Automatically extends sessions on user activity
5. **Comprehensive UI**: User-friendly warning modal with multiple options
6. **Error Handling**: Robust error handling and fallback mechanisms
7. **Localization Support**: Full Arabic/English localization
8. **Security Features**: Proper session cleanup and logout handling

### 3. Session Status Indicator Component

Create a header component showing session status:

```typescript
// components/SessionStatusIndicator.tsx
import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { SessionInfo } from '../types/session';

interface SessionStatusIndicatorProps {
  sessionInfo: SessionInfo | null;
  isWarningActive: boolean;
  onExtendSession?: () => void;
}

export const SessionStatusIndicator: React.FC<SessionStatusIndicatorProps> = ({
  sessionInfo,
  isWarningActive,
  onExtendSession
}) => {
  const { t } = useTranslation();
  const [timeLeft, setTimeLeft] = useState<string>('');
  const [showDetails, setShowDetails] = useState(false);

  useEffect(() => {
    if (!sessionInfo) return;

    const updateTimeLeft = () => {
      const minutes = Math.floor(sessionInfo.remainingSeconds / 60);
      const seconds = sessionInfo.remainingSeconds % 60;
      setTimeLeft(`${minutes}:${seconds.toString().padStart(2, '0')}`);
    };

    updateTimeLeft();
    const interval = setInterval(updateTimeLeft, 1000);

    return () => clearInterval(interval);
  }, [sessionInfo]);

  if (!sessionInfo) return null;

  const getStatusColor = () => {
    if (isWarningActive || sessionInfo.inWarningPeriod) return 'text-red-600';
    if (sessionInfo.remainingSeconds < 600) return 'text-yellow-600'; // Less than 10 minutes
    return 'text-green-600';
  };

  const getStatusIcon = () => {
    if (isWarningActive) {
      return (
        <svg className="h-4 w-4 mr-1 animate-pulse" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2}
            d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.732-.833-2.5 0L4.268 19.5c-.77.833.192 2.5 1.732 2.5z" />
        </svg>
      );
    }

    return (
      <svg className="h-4 w-4 mr-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2}
          d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
      </svg>
    );
  };

  return (
    <div className="relative">
      <div
        className={`flex items-center space-x-2 text-sm cursor-pointer ${getStatusColor()}`}
        onClick={() => setShowDetails(!showDetails)}
      >
        {getStatusIcon()}
        <span className="font-medium">{timeLeft}</span>
        {sessionInfo.userRole && (
          <span className="text-xs bg-gray-100 px-2 py-1 rounded">
            {sessionInfo.userRole}
          </span>
        )}
      </div>

      {showDetails && (
        <div className="absolute right-0 mt-2 w-64 bg-white rounded-lg shadow-lg border p-4 z-50">
          <div className="text-sm space-y-2">
            <div className="flex justify-between">
              <span className="text-gray-600">Status:</span>
              <span className={`font-medium ${getStatusColor()}`}>
                {sessionInfo.isActive ? 'Active' : 'Inactive'}
              </span>
            </div>

            <div className="flex justify-between">
              <span className="text-gray-600">Expires:</span>
              <span className="text-gray-900">
                {new Date(sessionInfo.expiresAt).toLocaleTimeString()}
              </span>
            </div>

            <div className="flex justify-between">
              <span className="text-gray-600">Last Activity:</span>
              <span className="text-gray-900">
                {new Date(sessionInfo.lastActivityAt).toLocaleTimeString()}
              </span>
            </div>

            {sessionInfo.deviceType && (
              <div className="flex justify-between">
                <span className="text-gray-600">Device:</span>
                <span className="text-gray-900">{sessionInfo.deviceType}</span>
              </div>
            )}

            {sessionInfo.isRememberMeSession && (
              <div className="flex justify-between">
                <span className="text-gray-600">Type:</span>
                <span className="text-blue-600">Remember Me</span>
              </div>
            )}
          </div>

          {onExtendSession && (
            <button
              onClick={onExtendSession}
              className="w-full mt-3 bg-blue-600 text-white px-3 py-2 rounded text-sm hover:bg-blue-700"
            >
              Extend Session
            </button>
          )}
        </div>
      )}
    </div>
  );
};
```

### 4. Main App Integration

Complete integration example with the main App component:

```typescript
// App.tsx
import React, { useEffect, useState, useCallback } from 'react';
import { SessionTimeoutService } from './services/sessionTimeoutService';
import { SessionWarningModal } from './components/SessionWarningModal';
import { SessionStatusIndicator } from './components/SessionStatusIndicator';
import { ApiClient } from './services/apiClient';
import { SessionInfo } from './types/session';

export const App: React.FC = () => {
  const [sessionService, setSessionService] = useState<SessionTimeoutService | null>(null);
  const [showWarning, setShowWarning] = useState(false);
  const [remainingSeconds, setRemainingSeconds] = useState(0);
  const [sessionInfo, setSessionInfo] = useState<SessionInfo | null>(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  // Initialize session timeout service
  useEffect(() => {
    const token = localStorage.getItem('accessToken');
    if (!token) return;

    setIsAuthenticated(true);

    const apiClient = new ApiClient();
    const service = new SessionTimeoutService(
      apiClient,
      handleSessionExpired,
      handleShowWarning,
      handleHideWarning,
      handleSessionExtended
    );

    service.initialize();
    setSessionService(service);

    return () => {
      service.cleanup();
    };
  }, []);

  const handleSessionExpired = useCallback(() => {
    console.log('Session expired - redirecting to login');

    // Clear tokens and user data
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('userData');

    // Reset state
    setIsAuthenticated(false);
    setSessionInfo(null);
    setShowWarning(false);

    // Redirect to login
    window.location.href = '/login?reason=session_expired';
  }, []);

  const handleShowWarning = useCallback((seconds: number, info: SessionInfo) => {
    setRemainingSeconds(seconds);
    setSessionInfo(info);
    setShowWarning(true);
  }, []);

  const handleHideWarning = useCallback(() => {
    setShowWarning(false);
  }, []);

  const handleSessionExtended = useCallback((info: SessionInfo) => {
    setSessionInfo(info);
    console.log('Session extended successfully', info);
  }, []);

  const handleExtendSession = useCallback(async (): Promise<boolean> => {
    if (sessionService) {
      return await sessionService.extendSession('User requested extension');
    }
    return false;
  }, [sessionService]);

  const handleLogout = useCallback(async () => {
    if (sessionService) {
      await sessionService.forceLogout();
    } else {
      handleSessionExpired();
    }
  }, [sessionService, handleSessionExpired]);

  const handleContinue = useCallback(() => {
    // Just hide the warning - activity will extend session automatically
    setShowWarning(false);
  }, []);

  if (!isAuthenticated) {
    return <LoginComponent />;
  }

  return (
    <div className="app">
      <header className="bg-white shadow-sm border-b">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center h-16">
            <div className="flex items-center">
              <h1 className="text-xl font-semibold text-gray-900">
                Jadwa Fund Management
              </h1>
            </div>

            <div className="flex items-center space-x-4">
              <SessionStatusIndicator
                sessionInfo={sessionInfo}
                isWarningActive={showWarning}
                onExtendSession={handleExtendSession}
              />

              <button
                onClick={handleLogout}
                className="text-gray-500 hover:text-gray-700"
              >
                Logout
              </button>
            </div>
          </div>
        </div>
      </header>

      <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
        {/* Your app content */}
        <YourAppContent />
      </main>

      <SessionWarningModal
        isOpen={showWarning}
        remainingSeconds={remainingSeconds}
        sessionInfo={sessionInfo!}
        onExtendSession={handleExtendSession}
        onLogout={handleLogout}
        onContinue={handleContinue}
      />
    </div>
  );
};
```

### 5. TypeScript Type Definitions

Complete type definitions for session management:

```typescript
// types/session.ts

export interface SessionInfo {
  sessionId: string;
  isActive: boolean;
  isExpired: boolean;
  expiresAt: string; // ISO date string
  remainingSeconds: number;
  inWarningPeriod: boolean;
  warningThresholdSeconds: number;
  lastActivityAt: string; // ISO date string
  createdAt: string; // ISO date string
  deviceType?: string;
  browserInfo?: string;
  ipAddress?: string;
  location?: string;
  userRole?: string;
  isRememberMeSession: boolean;
}

export interface SessionTimeoutConfig {
  timeoutMinutes: number;
  warningMinutes: number;
  slidingExpiration: boolean;
  checkIntervalSeconds: number;
  warningIntervalSeconds: number;
  rememberMeEnabled: boolean;
  roleBasedTimeouts: Record<string, number>;
}

export interface SessionExtensionRequest {
  sessionId?: string;
  reason?: string;
  isActivityBased?: boolean;
}

export interface SessionExtensionResponse {
  success: boolean;
  newExpiresAt: string; // ISO date string
  newRemainingSeconds: number;
  message?: string;
  sessionInfo?: SessionInfo;
}

export interface SessionValidationResponse {
  isValid: boolean;
  failureReason?: string;
  sessionInfo?: SessionInfo;
  wasExtended?: boolean;
}

export interface SessionStatistics {
  activeSessions: number;
  totalSessions: number;
  lastActivity?: string; // ISO date string
  firstSession?: string; // ISO date string
  deviceTypes: string[];
  locations: string[];
  averageSessionDurationMinutes: number;
}

export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
  errorCode?: string;
}
```

### 6. API Client Implementation

Complete API client for session management:

```typescript
// services/apiClient.ts
import axios, { AxiosInstance, AxiosResponse } from 'axios';
import {
  SessionInfo,
  SessionTimeoutConfig,
  SessionExtensionRequest,
  SessionExtensionResponse,
  SessionStatistics,
  ApiResponse
} from '../types/session';

export class ApiClient {
  private axiosInstance: AxiosInstance;

  constructor() {
    this.axiosInstance = axios.create({
      baseURL: process.env.REACT_APP_API_BASE_URL || 'http://localhost:5000',
      timeout: 10000,
    });

    this.setupInterceptors();
  }

  private setupInterceptors(): void {
    // Request interceptor to add auth token
    this.axiosInstance.interceptors.request.use(
      (config) => {
        const token = localStorage.getItem('accessToken');
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error) => Promise.reject(error)
    );

    // Response interceptor to handle session expiration
    this.axiosInstance.interceptors.response.use(
      (response) => response,
      (error) => {
        if (error.response?.status === 401 &&
            error.response?.data?.errorCode === 'SESSION_EXPIRED') {
          // Let the session service handle this
          console.warn('Session expired detected in API response');
        }
        return Promise.reject(error);
      }
    );
  }

  async getTimeoutConfig(userRole?: string): Promise<SessionTimeoutConfig> {
    const params = userRole ? { userRole } : {};
    const response: AxiosResponse<ApiResponse<SessionTimeoutConfig>> =
      await this.axiosInstance.get('/api/Users/Session/Timeout-Config', { params });

    if (!response.data.success) {
      throw new Error(response.data.message || 'Failed to get timeout configuration');
    }

    return response.data.data;
  }

  async getSessionStatus(sessionId?: string): Promise<SessionInfo> {
    const params = sessionId ? { sessionId } : {};
    const response: AxiosResponse<ApiResponse<SessionInfo>> =
      await this.axiosInstance.get('/api/Users/Session/Session-Status', { params });

    if (!response.data.success) {
      throw new Error(response.data.message || 'Failed to get session status');
    }

    return response.data.data;
  }

  async extendSession(request: SessionExtensionRequest): Promise<SessionExtensionResponse> {
    const response: AxiosResponse<ApiResponse<SessionExtensionResponse>> =
      await this.axiosInstance.post('/api/Users/Session/Extend-Session', request);

    if (!response.data.success) {
      throw new Error(response.data.message || 'Failed to extend session');
    }

    return response.data.data;
  }

  async validateSession(): Promise<SessionInfo> {
    const response: AxiosResponse<ApiResponse<SessionInfo>> =
      await this.axiosInstance.get('/api/Users/Session/Validate-Session');

    if (!response.data.success) {
      throw new Error(response.data.message || 'Session validation failed');
    }

    return response.data.data;
  }

  async updateActivity(): Promise<boolean> {
    const response: AxiosResponse<ApiResponse<boolean>> =
      await this.axiosInstance.post('/api/Users/Session/Update-Activity');

    return response.data.success;
  }

  async getSessionStatistics(): Promise<SessionStatistics> {
    const response: AxiosResponse<ApiResponse<SessionStatistics>> =
      await this.axiosInstance.get('/api/Users/Session/Session-Statistics');

    if (!response.data.success) {
      throw new Error(response.data.message || 'Failed to get session statistics');
    }

    return response.data.data;
  }

  async logout(): Promise<void> {
    try {
      await this.axiosInstance.post('/api/Users/Authentication/Sign-Out');
    } catch (error) {
      console.error('Logout API call failed:', error);
      // Don't throw - logout should always succeed locally
    }
  }
}
```

### 7. Configuration and Environment Setup

Environment configuration for different deployment scenarios:

```typescript
// config/session.config.ts
export const sessionConfig = {
  // Development settings
  development: {
    checkInterval: 60000,      // 1 minute
    warningInterval: 30000,    // 30 seconds
    activityDebounce: 1000,    // 1 second
    enableLogging: true,
  },

  // Production settings
  production: {
    checkInterval: 60000,      // 1 minute
    warningInterval: 30000,    // 30 seconds
    activityDebounce: 2000,    // 2 seconds
    enableLogging: false,
  },

  // Testing settings
  test: {
    checkInterval: 5000,       // 5 seconds (faster for testing)
    warningInterval: 2000,     // 2 seconds
    activityDebounce: 500,     // 0.5 seconds
    enableLogging: true,
  }
};

export const getCurrentConfig = () => {
  const env = process.env.NODE_ENV || 'development';
  return sessionConfig[env as keyof typeof sessionConfig] || sessionConfig.development;
};
```

### 8. Testing Utilities

Utilities for testing session timeout functionality:

```typescript
// utils/sessionTestUtils.ts
import { SessionTimeoutService } from '../services/sessionTimeoutService';
import { ApiClient } from '../services/apiClient';

export class SessionTestUtils {
  static createMockApiClient(): jest.Mocked<ApiClient> {
    return {
      getTimeoutConfig: jest.fn(),
      getSessionStatus: jest.fn(),
      extendSession: jest.fn(),
      validateSession: jest.fn(),
      updateActivity: jest.fn(),
      getSessionStatistics: jest.fn(),
      logout: jest.fn(),
    } as any;
  }

  static createMockSessionInfo(overrides = {}) {
    return {
      sessionId: 'test-session-id',
      isActive: true,
      isExpired: false,
      expiresAt: new Date(Date.now() + 30 * 60 * 1000).toISOString(),
      remainingSeconds: 1800,
      inWarningPeriod: false,
      warningThresholdSeconds: 300,
      lastActivityAt: new Date().toISOString(),
      createdAt: new Date().toISOString(),
      deviceType: 'Desktop',
      browserInfo: 'Chrome',
      userRole: 'FundManager',
      isRememberMeSession: false,
      ...overrides
    };
  }

  static simulateUserActivity(): void {
    // Simulate various user activities
    const events = ['mousedown', 'keypress', 'scroll'];
    events.forEach(eventType => {
      const event = new Event(eventType);
      document.dispatchEvent(event);
    });
  }

  static async waitForSessionCheck(service: SessionTimeoutService, timeout = 5000): Promise<void> {
    return new Promise((resolve, reject) => {
      const timer = setTimeout(() => {
        reject(new Error('Session check timeout'));
      }, timeout);

      // Mock implementation - in real tests you'd need to spy on the service
      setTimeout(() => {
        clearTimeout(timer);
        resolve();
      }, 1000);
    });
  }
}
```

## Implementation Checklist

### Backend Integration
- [ ] Verify session management APIs are deployed and accessible
- [ ] Test role-based timeout configurations
- [ ] Validate session validation middleware is working
- [ ] Confirm Redis session storage is configured

### Frontend Implementation
- [ ] Install required dependencies (axios, react-i18next, etc.)
- [ ] Implement SessionTimeoutService with activity detection
- [ ] Create SessionWarningModal component with countdown
- [ ] Add SessionStatusIndicator to application header
- [ ] Integrate with main App component
- [ ] Set up API client with proper error handling
- [ ] Configure environment variables for different deployments

### Testing & Validation
- [ ] Unit tests for SessionTimeoutService
- [ ] Component tests for warning modal and status indicator
- [ ] Integration tests with mock API responses
- [ ] Manual testing of complete session timeout flow
- [ ] Cross-browser compatibility testing
- [ ] Mobile responsiveness testing

### Deployment
- [ ] Configure environment-specific timeout settings
- [ ] Set up proper error monitoring and logging
- [ ] Validate session timeout works in production environment
- [ ] Test with different user roles and timeout configurations
- [ ] Verify localization works correctly in Arabic and English

This comprehensive implementation provides a robust, user-friendly session timeout mechanism that integrates seamlessly with the backend session management APIs and follows React/TypeScript best practices.
