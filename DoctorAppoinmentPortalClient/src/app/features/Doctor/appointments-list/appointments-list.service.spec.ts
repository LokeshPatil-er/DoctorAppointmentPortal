import { TestBed } from '@angular/core/testing';

import { AppointmentsListService } from './appointments-list.service';

describe('AppointmentsListService', () => {
  let service: AppointmentsListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AppointmentsListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
