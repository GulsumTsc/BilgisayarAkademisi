// Yalnızca görsel etkileşim. Cevap kontrolü ve can hesabı C# servisindedir.
document.querySelectorAll('.question-form').forEach(form => {
    const button = form.querySelector('button[type="submit"]');
    const radios = [...form.querySelectorAll('input[type="radio"]')];
    const update = () => { button.disabled = !radios.some(radio => radio.checked); };
    form.addEventListener('change', update);
    window.addEventListener('pageshow', update);
    update();
});
