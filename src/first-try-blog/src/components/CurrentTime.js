import React from 'react'

const CurrentTime = () => {
    const now = new Date();

    return (
        <div>
            <p>
            Current Time {now.toLocaleTimeString()}
            </p>
            <p>
                Expression 1 + 1 = { 1 + 1 }
            </p>
        </div>
    )
}

export default CurrentTime;