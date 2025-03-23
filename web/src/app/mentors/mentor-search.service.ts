import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

export interface MentorSearchResult {
  id: string;
  displayName: string;
  hourlyRate: number;
  rating: number;
  distanceKm: number | null;
}

export interface MentorFilters {
  subject?: string;
  meetingType?: 'Online' | 'InPerson' | 'Either';
  maxHourlyRate?: number;
  page?: number;
  pageSize?: number;
}

@Injectable({ providedIn: 'root' })
export class MentorSearchService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/mentors';

  search(filters: MentorFilters): Observable<MentorSearchResult[]> {
    let params = new HttpParams();

    for (const [key, value] of Object.entries(filters)) {
      if (value !== undefined && value !== null && value !== '') {
        params = params.set(key, String(value));
      }
    }

    return this.http.get<MentorSearchResult[]>(this.baseUrl, { params });
  }
}
