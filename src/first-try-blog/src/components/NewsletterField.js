import React, { useRef, useState } from 'react'

const NewsletterField = () => {
    const [email, setEmail] = useState();
    const emailRef = useRef();

    function changeEmailAddress(event) {
        setEmail(event.target.value);
    }

    function clearEmailInput() {
        //setEmail('');
        emailRef.current.value = '';
    }

  return (
    <div>
        <h1>Newsletter Form: {email}</h1>
        <input 
            type="email" 
            placeholder='Your email address'
            ref={emailRef}
            onChange={changeEmailAddress}
        />
        <button onClick={clearEmailInput}>Reset</button>
    </div>
  )
}

export default NewsletterField;