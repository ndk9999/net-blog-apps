import React, { useEffect, useState } from 'react'

const Alert = () => {
    const [alertDone, setAlertDone] = useState();

    useEffect(() => {
        console.log('Starting Alert Timer!')
        const timer = setTimeout(() => {
            console.log('Timer expired');
            setAlertDone(true);
        }, 2000);

        return function () {
            console.log('Cleanup!!!');
            clearTimeout(timer);
        }
    }, []);

    return (
        <div>
            {!alertDone && <p>Relax, you still got some time!</p>}
            {alertDone && <p>Time to get up!</p>}
        </div>
    )
}

export default Alert;