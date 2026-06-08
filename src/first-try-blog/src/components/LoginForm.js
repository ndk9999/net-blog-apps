import React, { useState } from 'react'

const LoginForm = () => {
    const [enteredEmail, setEnteredEmail] = useState('');
    const [emailIsValid, setEmailIsValid] = useState(true);

    const [enteredPassword, setEnteredPassword] = useState('');
    const [passwordIsValid, setPasswordIsValid] = useState(true);

    function changeEmailHandler(evt) {
        setEnteredEmail(evt.target.value);
    }

    function changePasswordHandler(evt) {
        setEnteredPassword(evt.target.value);
    }

    function submitFormHandler(evt) {
        evt.preventDefault();
        const isValidEmail = enteredEmail.includes('@');
        const isValidPwd = enteredPassword.trim().length >= 6;

        setEmailIsValid(isValidEmail);
        setPasswordIsValid(isValidPwd);

        if (!isValidEmail || !isValidPwd) {
            return;
        }

        console.log('Inputs are valid, form submitted!');
    }

    return (
        <form className='login' onSubmit={submitFormHandler}>
            Login

            <div>
                <label htmlFor="email" className={!emailIsValid && 'invalid'}>
                    Your email
                </label>
                <input
                    type="email"
                    id='email'
                    className={!emailIsValid && 'invalid'}
                    onChange={changeEmailHandler} />
            </div>

            <div>
                <label htmlFor="password" className={!passwordIsValid && 'invalid'}>
                    Password
                </label>
                <input
                    type="password"
                    id='password'
                    className={!passwordIsValid && 'invalid'}
                    onChange={changePasswordHandler} />
            </div>

            <button>Submit</button>
        </form>
    )
}

export default LoginForm;