import React from 'react'

const MathResult = ({ firstNumber, secondNumber, operator }) => {
    let result = 0;

    switch (operator) {
        case '+':
            result = firstNumber + secondNumber;
            break;
        case '-':
            result = firstNumber - secondNumber;
            break;
        case '*':
            result = firstNumber * secondNumber;
            break;
        case '/':
            result = secondNumber === 0 ? 0 : firstNumber / secondNumber;
            break;
        default:
            result = 0;
            break;
    }

    return (
        <div>Result: {result}</div>
    )
}

export default MathResult;