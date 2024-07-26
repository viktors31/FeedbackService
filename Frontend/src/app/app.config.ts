import { provideHttpClient } from '@angular/common/http';
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideClientHydration } from '@angular/platform-browser';
import { RECAPTCHA_SETTINGS, RecaptchaSettings } from 'ng-recaptcha';
import { provideNgxMask } from 'ngx-mask';
import { environment } from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideClientHydration(), provideNgxMask(), provideHttpClient(),
    {
      provide: RECAPTCHA_SETTINGS,
      useValue: {
        siteKey: environment.siteKeyV2,
      } as RecaptchaSettings,
    }
  ]
};
