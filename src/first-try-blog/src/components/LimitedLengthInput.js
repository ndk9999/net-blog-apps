import React, { useState } from 'react'

const LimitedLengthInput = () => {
    const [message, setMessage] = useState('');
    const numChars = message.length;
    const maxLength = 300;

  return (
    <div>
        <p>Please enter maximum {maxLength} characters</p>
        <textarea onChange={(evt) => setMessage(evt.target.value)}></textarea>
        <p>You have entered {numChars}/{maxLength} characters</p>
    </div>
  )
}

export default LimitedLengthInput;