import { OnInit } from '@angular/core';
import { EditorBase } from '@sitecore/ma-core';
export declare class DaysAndLifecycleEditorComponent extends EditorBase implements OnInit {
    days: number;
    lifecycle: number;
    valueLifecycle: any;
    ngOnInit(): void;
    increaseValue(): void;
    decreaseValue(): void;
    serialize(): any;
}
