import { TestBed } from '@angular/core/testing';

import { AppointmentFormService } from './appointment-form.service';

describe('AppointmentFormService', () => {
  let service: AppointmentFormService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AppointmentFormService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
