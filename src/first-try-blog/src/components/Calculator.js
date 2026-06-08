import React, { useState } from 'react'
import MathResult from './MathResult';
import Operation from './Operation';

const Calculator = () => {
    const [expression, setExpression] = useState({
        firstNumber: 0,
        secondNumber: 0,
        operator: '*'
    });

    function updateFirstNumber(evt) {
        setExpression(prevState => {
            return {
                ...prevState,
                firstNumber: +evt.target.value
            };
        })
    } 

    function updateSecondNumber(evt) {
        setExpression(prevState => {
            return {
                ...prevState,
                secondNumber: +evt.target.value
            };
        })
    } 

    function updateOperator(evt) {
        setExpression(prevState => {
            return {
                ...prevState,
                operator: evt.target.value
            };
        })
    } 

  return (
    <div>
        <Operation
            expression={expression}
            onFirstNumberChanged={updateFirstNumber}
            onSecondNumberChanged={updateSecondNumber}
            onOperatorChanged={updateOperator}
        />

        <MathResult 
            firstNumber={expression.firstNumber} 
            secondNumber={expression.secondNumber}
            operator={expression.operator}
        />
    </div>
  )
}

export default Calculator;