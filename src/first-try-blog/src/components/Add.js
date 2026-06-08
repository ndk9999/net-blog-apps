import React, { useState } from 'react'

const Add = () => {
    const [firstNumber, setFirstNumber] = useState(0.0);
    const [secondNumber, setSecondNumber] = useState(0.0);

    const sum = firstNumber + secondNumber;

    return (
        <div className='mb-2'>
            <input
                type="number"
                onChange={evt => setFirstNumber(parseFloat(evt.target.value || '0'))}
            />
            +
            <input
                type="number"
                onChange={evt => setSecondNumber(parseFloat(evt.target.value || '0'))}
            />
            =
            <span>{sum}</span>
        </div>
    )
}

export default Add;