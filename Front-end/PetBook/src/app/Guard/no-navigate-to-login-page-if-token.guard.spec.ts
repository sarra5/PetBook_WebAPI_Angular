import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { noNavigateToLoginPageIfTokenGuard } from './no-navigate-to-login-page-if-token.guard';

describe('noNavigateToLoginPageIfTokenGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => noNavigateToLoginPageIfTokenGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
