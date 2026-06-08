import React from 'react'

const Operation = ({ expression, onFirstNumberChanged, onSecondNumberChanged, onOperatorChanged }) => {
    return (
        <div>
            <input type="number" value={expression.firstNumber} onChange={onFirstNumberChanged} />

            <select onChange={onOperatorChanged} value={expression.operator}>
                <option value="+">+</option>
                <option value="-">-</option>
                <option value="*">*</option>
                <option value="/">/</option>
            </select>

            <input type="number" value={expression.secondNumber} onChange={onSecondNumberChanged} />
        </div>
    )
}

export default Operation;