import React, { useState } from 'react'
import Alert from '../components/Alert';
import DynamicAlert from '../components/DynamicAlert';

const Dashboard = () => {
  const [showAlert, setShowAlert] = useState();

  function showAlertHandler() {
    setShowAlert(prevVal => !prevVal);
  }

  return (
    <>
      <div>Dashboard</div>

      <div>
        <button onClick={showAlertHandler}>
          {showAlert ? 'Hide' : 'Show'}
        </button>
        {showAlert && <Alert />}

        <DynamicAlert />
      </div>
    </>
  )
}

export default Dashboard;