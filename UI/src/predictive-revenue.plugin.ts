import { Plugin } from '@sitecore/ma-core';
import { PredictiveRevenueActivity } from './predictive-revenue/predictive-revenue-activity';
import { PredictiveRevenueModuleNgFactory } from '../codegen/predictive-revenue/predictive-revenue-module.ngfactory';
import { DaysAndLifecycleEditorComponent } from '../codegen/predictive-revenue/editor/daysandlifecycle-editor.component';

// Use the @Plugin decorator to define all the activities the module contains.
@Plugin({
    activityDefinitions: [
        {
            // The ID must match the ID of the activity type description definition item in the CMS.
            id: 'BD1D0B32-6E3F-4963-8212-D3016C9BDABC',
            activity: PredictiveRevenueActivity,
            editorComponenet: DaysAndLifecycleEditorComponent,
            editorModuleFactory: PredictiveRevenueModuleNgFactory
        }
    ]
})
export default class PredictiveRevenuePlugin { }
