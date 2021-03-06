import { Component, OnInit, OnChanges, Injector } from '@angular/core';
import {
  EditorBase
} from '@sitecore/ma-core';

@Component({
  selector: 'daysandlifecycle-editor',
  template: `
      <section class="content">
        <div class="form-group">
            <div class="row daysandlifecycle-editor">
                <label class="col-6 title">Days (between 1 and 31)</label>
                <div class="col-6">
                    <span class="minus-icon" (click)="decreaseValue()">-</span>
                    <input type="number" min="1" max="31" class="form-control" [(ngModel)]="days" />
                    <span class="plus-icon" (click)="increaseValue()">+</span>
                </div>
                <label class="col-6 title">Lifecycle</label>
                <div class="col-6">
                    <select class="form-control" [(ngModel)]="lifecycle">
                        <option [value]="0">Low</option>
                        <option [value]="1">Medium</option>
                        <option [value]="2">High</option>
                    </select>
                </div>
            </div>
        </div>
      </section>
    `,
  //CSS Styles are ommitted for brevity
  styles: ['']
})

export class DaysAndLifecycleEditorComponent extends EditorBase implements OnInit {

  days: number;
  lifecycle: number;
  valueLifecycle: any;

  ngOnInit(): void {
    console.log(this.model);
    this.days = this.model ? this.model.Days || 0 : 1;

    this.lifecycle = 0;
    if (!this.model && this.model.Lifecycle == 'Low' || this.model.Lifecycle == 'low') {
      this.lifecycle = 0;
    }
    else if (!this.model && this.model.Lifecycle == 'Medium' || this.model.Lifecycle == 'medium') {
      this.lifecycle = 1;
    }
    else if (!this.model && this.model.Lifecycle == 'High' || this.model.Lifecycle == 'high') {
      this.lifecycle = 2;
    }
  }

  /**
  * Increases the count by 1. Bound to the '+' button.
  */
  increaseValue() {
    this.days++;
  }

  /**
  * Decreases the count by 1. Bound to the '-' button.
  */
  decreaseValue() {
    this.days--;
  }

  serialize(): any {
    if (this.lifecycle == 0) {
      this.valueLifecycle = 'Low';
    }
    else if (this.lifecycle == 1) {
      this.valueLifecycle = 'Medium';
    }
    else if (this.lifecycle == 2) {
      this.valueLifecycle = 'High';
    }
    return {
      Days: this.days,
      Lifecycle: this.valueLifecycle
    };
  }
}