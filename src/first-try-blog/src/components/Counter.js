import React, { useState } from 'react'

const Counter = () => {
    const [counter, setCounter] = useState(0);

    function increaseCounter() {
        setCounter((prevCounter) => prevCounter + 1);
    }

  return (
    <div>
        <p>Current value: {counter}</p>
        <button type='button' onClick={increaseCounter}>Increment</button>
    </div>
  )
}

export default Counter;