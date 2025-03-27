import { Component, inject, signal } from '@angular/core';
import { MentorSearchService, MentorSearchResult } from './mentor-search.service';

@Component({
  selector: 'mandat-mentor-list',
  standalone: true,
  template: `
    <section class="mentor-list">
      <h2>Find a mentor</h2>

      @if (loading()) {
        <p class="muted">Searching...</p>
      } @else if (mentors().length === 0) {
        <p class="muted">No mentors match those filters yet.</p>
      } @else {
        <ul>
          @for (mentor of mentors(); track mentor.id) {
            <li>
              <strong>{{ mentor.displayName }}</strong>
              <span>{{ mentor.rating | number: '1.1-1' }} stars</span>
              <span>{{ mentor.hourlyRate | currency: 'GBP' }}/hr</span>
              @if (mentor.distanceKm !== null) {
                <span>{{ mentor.distanceKm | number: '1.0-0' }} km away</span>
              }
            </li>
          }
        </ul>
      }
    </section>
  `
})
export class MentorListComponent {
  private readonly service = inject(MentorSearchService);

  readonly mentors = signal<MentorSearchResult[]>([]);
  readonly loading = signal(false);

  search(subject?: string): void {
    this.loading.set(true);
    this.service.search({ subject, pageSize: 20 }).subscribe({
      next: results => this.mentors.set(results),
      complete: () => this.loading.set(false)
    });
  }
}
