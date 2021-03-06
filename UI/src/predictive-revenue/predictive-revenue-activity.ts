import { ConditionItem } from '@sitecore/ma-core';

export class PredictiveRevenueActivity extends ConditionItem {
	
    get isDefined(): boolean {
        return Boolean(this.editorParams.Days || this.editorParams.Lifecycle);
    }

    getVisual(): string {
        const title = this.isDefined ? 'Predictive revenue' : '';
        const cssClass = this.isDefined ? '' : 'undefined';
        
        return `
            <div class="viewport-custom-listener listener ${cssClass}">
                <span class="icon">
                    <img src="/~/icon/officewhite/32x32/astrologer.png" />
                </span>
                <p class="text with-subtitle" title="Predictive revenue">
                    ${title}
                    <small class="subtitle" title="Days: ${this.editorParams.Days} and Lifecycle: ${this.editorParams.Lifecycle}">Days: ${this.editorParams.Days} and Lifecycle: ${this.editorParams.Lifecycle}</small>
                </p>
            </div>
        `;
    }
}