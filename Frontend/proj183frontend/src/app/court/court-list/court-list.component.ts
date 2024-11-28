import { Component, OnInit, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { CourtViewModel } from '../../models/shared.models';
import { CourtService } from '../../services/court.service';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-court-list',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatDividerModule],
  templateUrl: './court-list.component.html',
  styleUrl: './court-list.component.scss',
})
export class CourtListComponent implements OnInit {
  public courts$!: Observable<CourtViewModel[]>;

  private readonly courtService = inject(CourtService);

  public ngOnInit(): void {
    this.courts$ = this.courtService.getCourts();
  }
}
