import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DaysAndLifecycleEditorComponent } from './editor/daysandlifecycle-editor.component';

@NgModule({
    imports: [
        CommonModule, FormsModule
    ],
    declarations: [DaysAndLifecycleEditorComponent],
    entryComponents: [DaysAndLifecycleEditorComponent]
})
export class PredictiveRevenueModule { }
