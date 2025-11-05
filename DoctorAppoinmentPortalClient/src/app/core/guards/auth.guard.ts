import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ToastService } from '../services/toast.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const toastService = inject(ToastService);

  const token = authService.getToken();

  if (!token) {
    debugger;
    toastService.warningToastr(
      'Your session has expired. Please log in to continue.',
      'Session Expired'
    );
    authService.logout();
    return false;
  }

  if (authService.isTokenExpired()) {
    authService.logout();
    toastService.infoToastr(
      'Your login session has timed out. Please log in again.',
      'Session Timeout'
    );
    
    return false;
  }

  const userRole = authService.getUserRole();
  if (!userRole) {
    toastService.errorToastr(
      'User role missing. Please contact administrator.',
      'Access Error'
    );
    authService.logout();
    return false;
  }

  const requiredRoles =
    route.data?.['roles'] ?? route.parent?.data?.['roles'] ?? [];

  if (requiredRoles.length > 0 && !requiredRoles.includes(userRole)) {
    toastService.warningToastr(
      'You do not have permission to access this page.',
      'Access Restricted'
    );
    authService.logout();
  }

  return true;
};
