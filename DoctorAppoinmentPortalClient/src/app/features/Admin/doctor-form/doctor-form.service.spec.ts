import { TestBed } from '@angular/core/testing';

import { DoctorFormService } from './doctor-form.service';

describe('DoctorFormService', () => {
  let service: DoctorFormService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DoctorFormService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
