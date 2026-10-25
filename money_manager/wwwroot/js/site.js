//View->Transactions->Create And Edit Views use this file
const SelectListItemsCategories = (urlGetCategories = '') => {
    const operationTypeSelect = document.getElementById('OperationTypeId');

    operationTypeSelect.addEventListener("change", async () => {

        const selectedValue = operationTypeSelect.value;

        const response = await fetch(urlGetCategories, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: selectedValue
        });

        const jsonResponse = await response.json();

        const options = jsonResponse.map(category => `<option value="${category.value}">${category.text}</option>`).join('');
        const categorySelect = document.getElementById('CategoryId');
        categorySelect.innerHTML = options;
    });
}