import { ConditionItem } from '@sitecore/ma-core';

export class PredictiveRevenueActivity extends ConditionItem {
	
    get isDefined(): boolean {
        return Boolean(this.editorParams.days || this.editorParams.lifecycle);
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
                    <small class="subtitle" title="Days: ${this.editorParams.days} and Lifecycle: ${this.editorParams.lifecycle}">Days: ${this.editorParams.days} and Lifecycle: ${this.editorParams.lifecycle}</small>
                </p>
            </div>
        `;
    }
}