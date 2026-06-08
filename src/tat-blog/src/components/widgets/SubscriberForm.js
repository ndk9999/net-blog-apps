import React, { useRef, useState } from 'react';

const SubscribeForm = () => {
    //const [email, setEmail] = useState('');
    const emailRef = useRef();
    const [message, setMessage] = useState('');

    function subscribeHandler(evt) {
        evt.preventDefault();

        setMessage('Đang gửi dữ liệu ...');

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email: emailRef.current.value })
        };

        fetch('https://localhost:7076/subscribers', requestOptions)
            .then(response => response.json())
            .then(data => {
                setMessage(data);
                emailRef.current.value = '';
            })
            .catch(err => {
                setMessage("Đã có lỗi xảy ra");
            });
    }

    return (
        <div>
            <h1>Subscribe to our Blog</h1>
            {message && (
                <p>{message}</p>
            )}
            <form onSubmit={subscribeHandler}>
                <label> Your email:
                    {/* <input type="email"
                        onChange={(e) => setEmail(e.target.value)} /> */}
                    <input type="email" ref={emailRef} />
                </label>
                
                <button type="submit">Submit</button>
            </form>
        </div>
    )
}

export default SubscribeForm;