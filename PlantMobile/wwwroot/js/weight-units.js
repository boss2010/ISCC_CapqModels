(() => {
    const selector = 'input[name$=".SampleSize"], input[name$=".MaterialAmount"], #newSampleSize';

    // قيمة خام للحساب والإرسال
    const format = value => Number(value.toFixed(12)).toString();

    // عرض فقط بالفواصل
    const formatDisplay = value => {
        if (!Number.isFinite(value)) return '—';

        return value.toLocaleString('en-US', {
            maximumFractionDigits: 12,
            useGrouping: true
        });
    };

    const initialize = () => {
        document.querySelectorAll(selector).forEach(input => {
            if (input.dataset.weightReady) return;

            input.dataset.weightReady = 'true';

            const wrapper = document.createElement('div');
            wrapper.className = 'weight-control weight-editor';

            input.before(wrapper);
            wrapper.append(input);

            const unit = document.createElement('select');
            const isSample = input.id === 'newSampleSize' || input.name.endsWith('.SampleSize');
            unit.className = 'weight-unit';
            unit.setAttribute('aria-label', isSample ? 'وحدة كمية العينة' : 'وحدة الوزن');

            unit.innerHTML =
                '<option value="1">كيلوجرام</option>' +
                '<option value="1000">طن</option>' +
                '<option value="0.001">جرام</option>';
            if (isSample) unit.add(new Option('عدد', 'count'));

            wrapper.append(unit);

            input.dataset.weightFactor = '1';
            unit.value = input.dataset.quantityUnit === 'count' ? 'count' : '1';
            unit.disabled = input.disabled || input.readOnly;

            const baseMin = input.min;
            const baseStep = input.step;
            if (unit.value === 'count') { input.min = '1'; input.step = '1'; }

            unit.addEventListener('change', () => {
                const previousFactor = Number(input.dataset.weightFactor);
                const isCount = unit.value === 'count';
                const wasCount = input.dataset.quantityUnit === 'count';
                const factor = isCount ? 1 : Number(unit.value);

                if (input.value !== '') {
                    // إزالة أي فواصل قبل الحساب احتياطياً
                    const currentValue = Number(String(input.value).replace(/,/g, ''));

                    if (Number.isFinite(currentValue) && !isCount && !wasCount) {
                        input.value = format(
                            currentValue * previousFactor / factor
                        );
                    }
                }

                input.dataset.weightFactor = String(factor);
                input.dataset.quantityUnit = isCount ? 'count' : 'kg';

                if (baseMin) {
                    input.min = isCount ? '1' : format(Number(baseMin) / factor);
                }

                if (baseStep && baseStep !== 'any') {
                    input.step = isCount ? '1' : format(Number(baseStep) / factor);
                }
            });
        });
    };

    document.addEventListener('click', event => {
        if (event.target.closest('.weight-unit')) {
            event.stopPropagation();
        }
    });

    document.addEventListener('change', event => {
        if (!event.target.matches('.weight-unit')) return;

        const wrapper = event.target.closest('[data-weight-kg]');
        if (!wrapper) return;

        const raw = wrapper.dataset.weightKg;
        const valueElement = wrapper.querySelector('[data-weight-value]');

        if (!valueElement) return;

        valueElement.textContent =
            raw === ''
                ? '—'
                : formatDisplay(
                    Number(raw) / Number(event.target.value)
                );
    });

    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('formdata', event => {
            [...form.elements].filter(input => input.matches(selector)).forEach(input => {
                if (!input.name || input.disabled || input.value === '') return;

                const cleanValue = Number(
                    String(input.value).replace(/,/g, '')
                );

                if (!Number.isFinite(cleanValue)) return;
                if (input.name.endsWith('.SampleSize')) {
                    event.formData.set(input.name.replace(/\.SampleSize$/, '.QuantityUnit'), input.dataset.quantityUnit === 'count' ? 'count' : 'kg');
                }

                event.formData.set(
                    input.name,
                    format(
                        cleanValue *
                        Number(input.dataset.weightFactor || 1)
                    )
                );
            });
        });
    });

    initialize();

    const samples = document.getElementById('newSamplesContainer');

    if (samples) {
        new MutationObserver(initialize).observe(samples, {
            childList: true,
            subtree: true
        });
    }
})();
