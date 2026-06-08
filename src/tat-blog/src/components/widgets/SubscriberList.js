import axios from 'axios';
import React, { useState, useEffect } from 'react';

const SubscriberList = () => {
    const [subscribers, setSubscribers] = useState([]);
    const [message, setMessage] = useState('');

    useEffect(() => {
        setMessage('Đang tải dữ liệu ...');

        axios.get(`https://localhost:7076/subscribers`)
            .then(response => {
                setSubscribers(response.data);
                setMessage(`Có ${response.data.length} người đăng ký mới`);
            })
            .catch(err => {
                console.log(err);
                setMessage('Đã có lỗi xảy ra');
            });
            
        //fetch(`https://localhost:7076/subscribers`)
            // .then(response => response.json())
            // .then(data => {
            //     setSubscribers(data);
            //     setMessage(`Có ${data.length} người đăng ký mới`);
            // })
            // .catch(err => {
            //     console.log(err);
            //     setMessage('Đã có lỗi xảy ra');
            // });
    }, []);

    
    return (
        
        <div className="list-wrapper">
            <p>{message}</p>

            {subscribers.map((item) => (
               <div className="list-item" key={item.id}>
                    <h1>{item.email}</h1>
                    <p>{new Date(item.subscribedDate).toLocaleDateString()}</p>
                </div>
            ))}
        </div>
    )

}

export default SubscriberList;