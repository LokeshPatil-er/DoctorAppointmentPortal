import { TestBed } from '@angular/core/testing';

import { DoctorsListService } from './doctors-list.service';

describe('DoctorsListService', () => {
  let service: DoctorsListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DoctorsListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
