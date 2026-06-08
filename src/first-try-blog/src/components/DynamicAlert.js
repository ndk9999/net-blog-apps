import React, { useCallback, useEffect, useState } from 'react'

const DynamicAlert = () => {
    const [alertMsg, setAlertMsg] = useState('Expired!');

    function changeAlertMsgHandler(evt) {
        setAlertMsg(evt.target.value);
    }

    // function setAlert() {
    //     console.log('Display alert message');
    //     return setTimeout(function () {
    //         console.log(alertMsg);
    //     }, 2000);
    // }

    // useEffect(function () {
    //     const timer = setAlert();
    //     return function () {
    //         clearTimeout(timer);
    //     }
    // }, [setAlert]);

    useEffect(function () {
        function setAlert() {
            console.log('Display alert message');
            return setTimeout(function () {
                console.log(alertMsg);
            }, 2000);
        }

        const timer = setAlert();
        return function () {
            clearTimeout(timer);
        }
    }, [alertMsg]);

    useEffect(() => {
        console.log('Run!');
    }, []);

  return (
    <input type="text" onChange={changeAlertMsgHandler} />
  )
}

export default DynamicAlert;