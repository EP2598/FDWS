// Villager Table Calculator - JavaScript Functions
let rowCounter = 1;

// Prevent non-numeric characters during keypress
function isNumericKey(event) {
    const charCode = event.which ? event.which : event.keyCode;
    // Allow backspace, delete, tab, escape, enter
    if ([8, 9, 27, 13, 46].indexOf(charCode) !== -1 ||
        // Allow Ctrl+A, Ctrl+C, Ctrl+V, Ctrl+X
        (charCode === 65 && event.ctrlKey === true) ||
        (charCode === 67 && event.ctrlKey === true) ||
        (charCode === 86 && event.ctrlKey === true) ||
        (charCode === 88 && event.ctrlKey === true)) {
        return true;
    }
    // Ensure that it is a number and stop the keypress
    if ((charCode < 48 || charCode > 57)) {
        event.preventDefault();
        return false;
    }
    return true;
}

// Validate input on change/input events
function validateNumericInput(input) {
    // Remove any non-numeric characters except for temporarily allowed ones
    let value = input.value;
    
    // Remove any character that's not a digit
    value = value.replace(/[^0-9]/g, '');
    
    // Update the input value
    input.value = value;
    
    // Validate ranges
    validateInputRange(input);
    
    // Remove invalid styling if value is now valid
    if (value && !isNaN(parseInt(value))) {
        input.classList.remove('is-invalid');
        input.classList.add('is-valid');
    } else if (value === '') {
        input.classList.remove('is-invalid', 'is-valid');
    }
}

// Validate input ranges
function validateInputRange(input) {
    const value = parseInt(input.value);
    
    if (input.classList.contains('age-input')) {
        if (value < 0 || value > 200) {
            input.classList.add('is-invalid');
            input.classList.remove('is-valid');
            return false;
        }
    } else if (input.classList.contains('year-input')) {
        if (value < 1 || value > 9999) {
            input.classList.add('is-invalid');
            input.classList.remove('is-valid');
            return false;
        }
    }
    
    return true;
}

function addTableRow() {
    const tableBody = document.getElementById('villagerTableBody');
    const newRow = document.createElement('tr');
    newRow.setAttribute('data-row-index', rowCounter);
    
    newRow.innerHTML = `
        <td class="align-middle">
            <span class="badge bg-primary fs-6">${rowCounter + 1}</span>
        </td>
        <td>
            <input type="number" 
                   class="form-control age-input" 
                   placeholder="Enter age" 
                   min="0" 
                   max="200"
                   step="1"
                   pattern="[0-9]*"
                   oninput="validateNumericInput(this)"
                   onkeypress="return isNumericKey(event)">
        </td>
        <td>
            <input type="number" 
                   class="form-control year-input" 
                   placeholder="Enter year" 
                   min="1" 
                   max="9999"
                   step="1"
                   pattern="[0-9]*"
                   oninput="validateNumericInput(this)"
                   onkeypress="return isNumericKey(event)">
        </td>
        <td>
            <button class="btn btn-sm btn-outline-danger" 
                    type="button" 
                    onclick="removeTableRow(${rowCounter})"
                    title="Remove this villager">
                <i class="fas fa-trash"></i> Remove
            </button>
        </td>
    `;
    
    tableBody.appendChild(newRow);
    rowCounter++;
    
    // Add a subtle animation
    newRow.style.opacity = '0';
    newRow.style.transform = 'translateY(-10px)';
    setTimeout(() => {
        newRow.style.transition = 'all 0.3s ease';
        newRow.style.opacity = '1';
        newRow.style.transform = 'translateY(0)';
    }, 10);
}

function removeTableRow(index) {
    const row = document.querySelector(`tr[data-row-index="${index}"]`);
    if (row) {
        // Add fade-out animation
        row.style.transition = 'all 0.3s ease';
        row.style.opacity = '0';
        row.style.transform = 'translateX(-20px)';
        
        setTimeout(() => {
            row.remove();
            updateRowNumbers();
        }, 300);
    }
}

function updateRowNumbers() {
    const rows = document.querySelectorAll('#villagerTableBody tr');
    rows.forEach((row, index) => {
        const badge = row.querySelector('.badge');
        if (badge) {
            badge.textContent = index + 1;
        }
    });
}

function clearAllRows() {
    const tableBody = document.getElementById('villagerTableBody');
    tableBody.innerHTML = `
        <tr data-row-index="0">
            <td class="align-middle">
                <span class="badge bg-primary fs-6">1</span>
            </td>
            <td>
                <input type="number" 
                       class="form-control age-input" 
                       placeholder="Enter age" 
                       min="0" 
                       max="200"
                       step="1"
                       pattern="[0-9]*"
                       oninput="validateNumericInput(this)"
                       onkeypress="return isNumericKey(event)">
            </td>
            <td>
                <input type="number" 
                       class="form-control year-input" 
                       placeholder="Enter year" 
                       min="1" 
                       max="9999"
                       step="1"
                       pattern="[0-9]*"
                       oninput="validateNumericInput(this)"
                       onkeypress="return isNumericKey(event)">
            </td>
            <td>
                <button class="btn btn-sm btn-outline-danger" 
                        type="button" 
                        onclick="removeTableRow(0)"
                        title="Remove this villager">
                    <i class="fas fa-trash"></i> Remove
                </button>
            </td>
        </tr>
    `;
    rowCounter = 1;
    document.getElementById('result').innerHTML = '';
}

function loadExampleData() {
    clearAllRows();
    
    const exampleData = [
        [65, 2020],
        [72, 2019],
        [58, 2021]
    ];
    
    const tableBody = document.getElementById('villagerTableBody');
    tableBody.innerHTML = '';
    
    exampleData.forEach((data, index) => {
        const newRow = document.createElement('tr');
        newRow.setAttribute('data-row-index', index);
        
        newRow.innerHTML = `
            <td class="align-middle">
                <span class="badge bg-primary fs-6">${index + 1}</span>
            </td>
            <td>
                <input type="number" 
                       class="form-control age-input" 
                       placeholder="Enter age" 
                       min="0" 
                       max="200"
                       step="1"
                       pattern="[0-9]*"
                       value="${data[0]}"
                       oninput="validateNumericInput(this)"
                       onkeypress="return isNumericKey(event)">
            </td>
            <td>
                <input type="number" 
                       class="form-control year-input" 
                       placeholder="Enter year" 
                       min="1" 
                       max="9999"
                       step="1"
                       pattern="[0-9]*"
                       value="${data[1]}"
                       oninput="validateNumericInput(this)"
                       onkeypress="return isNumericKey(event)">
            </td>
            <td>
                <button class="btn btn-sm btn-outline-danger" 
                        type="button" 
                        onclick="removeTableRow(${index})"
                        title="Remove this villager">
                    <i class="fas fa-trash"></i> Remove
                </button>
            </td>
        `;
        
        tableBody.appendChild(newRow);
    });
    
    rowCounter = exampleData.length;
}

async function submitData() {
    const rows = document.querySelectorAll('#villagerTableBody tr');
    const listInputs = [];
    
    let hasError = false;
    let errorMessages = [];
    
    rows.forEach((row, index) => {
        const ageInput = row.querySelector('.age-input');
        const yearInput = row.querySelector('.year-input');
        
        const ageValue = ageInput.value.trim();
        const yearValue = yearInput.value.trim();
        
        // Enhanced validation
        const age = parseInt(ageValue);
        const year = parseInt(yearValue);
        
        // Check if values are empty
        if (!ageValue || !yearValue) {
            hasError = true;
            ageInput.classList.add('is-invalid');
            yearInput.classList.add('is-invalid');
            errorMessages.push(`Row ${index + 1}: Both age and year are required`);
            return;
        }
        
        // Check if values are valid numbers
        if (isNaN(age) || isNaN(year)) {
            hasError = true;
            if (isNaN(age)) ageInput.classList.add('is-invalid');
            if (isNaN(year)) yearInput.classList.add('is-invalid');
            errorMessages.push(`Row ${index + 1}: Invalid numeric values`);
            return;
        }
        
        // Check ranges
        if (age < 0 || age > 200) {
            hasError = true;
            ageInput.classList.add('is-invalid');
            errorMessages.push(`Row ${index + 1}: Age must be between 0 and 200`);
        }
        
        if (year < 1 || year > 9999) {
            hasError = true;
            yearInput.classList.add('is-invalid');
            errorMessages.push(`Row ${index + 1}: Year must be between 1 and 9999`);
        }
        
        // Additional business logic validation
        if (year < age) {
            hasError = true;
            yearInput.classList.add('is-invalid');
            errorMessages.push(`Row ${index + 1}: Year of death cannot be less than age of death`);
        }
        
        if (!hasError || errorMessages.length === 0) {
            ageInput.classList.remove('is-invalid');
            yearInput.classList.remove('is-invalid');
            ageInput.classList.add('is-valid');
            yearInput.classList.add('is-valid');
            listInputs.push([age, year]);
        }
    });
    
    if (hasError) {
        const uniqueErrors = [...new Set(errorMessages)];
        document.getElementById('result').innerHTML = 
            `<div class="alert alert-danger">
                <i class="fas fa-exclamation-triangle"></i> <strong>Validation Errors:</strong><br>
                ${uniqueErrors.map(error => `• ${error}`).join('<br>')}
            </div>`;
        return;
    }
    
    if (listInputs.length === 0) {
        document.getElementById('result').innerHTML = 
            `<div class="alert alert-warning">
                <i class="fas fa-exclamation-circle"></i> Please add at least one villager data.
            </div>`;
        return;
    }
    
    try {
        const response = await fetch('/Home/GetResult', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(listInputs)
        });
        
        const result = await response.json();
        
        if (result.success) {
            document.getElementById('result').innerHTML = 
                `<div class="alert alert-success">
                    <i class="fas fa-check-circle"></i> <strong>Success!</strong><br>
                    <strong>Average Death Calculation:</strong> ${result.message}<br>
                </div>`;
        } else {
            document.getElementById('result').innerHTML = 
                `<div class="alert alert-danger">
                    <i class="fas fa-times-circle"></i> <strong>Error:</strong> ${result.message}
                </div>`;
        }
    } catch (error) {
        document.getElementById('result').innerHTML = 
            `<div class="alert alert-danger">
                <i class="fas fa-exclamation-triangle"></i> <strong>Network Error:</strong> ${error.message}
            </div>`;
    }
}