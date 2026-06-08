import React, { useState } from 'react'

const EmailInput = () => {
    const [errorMessage, setErrorMessage] = useState('');

    function evaluateEmail(event) {
        const enteredEmail = event.target.value;
        if (enteredEmail.trim() === '' || !enteredEmail.includes('@')) {
            setErrorMessage('The entered email address is invalid');
        } else {
            setErrorMessage('No error');
        }
    }
console.log(errorMessage);
    return (
        <div>
            <input type="email" placeholder='Your email' onBlur={evaluateEmail} />
            <p>
                {errorMessage}
            </p>
        </div>
    )
}
// setTimeout(() => {
//     const emailInput = document.querySelector('#emailInput');
//     const emailError = document.querySelector('#emailError');

//     function evaluateEmail(event) {
//         const enteredEmail = event.target.value;
//         console.log(enteredEmail);
//         if (enteredEmail.trim() === '' || !enteredEmail.includes('@')) {
//             emailError.textContent = 'The entered email address is invalid';
//         } else {
//             emailError.textContent = '';
//         }
//     }

//     emailInput.addEventListener('blur', evaluateEmail);
// }, 2000);
export default EmailInput;