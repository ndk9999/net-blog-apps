import React, { useEffect, useState } from 'react'

const CandyList = () => {
    const [candies, setCandies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(false);

    useEffect(() => {
        function loadCandies () {
            const apiEndpoint = `https://localhost:7076/candies`;
            fetch(apiEndpoint)
                .then(response => response.json())
                .then(data => {
                    setLoading(false);
                    setCandies(data);
                })
                .catch(err => {
                    setLoading(false);
                    setError(true)
                });
        }

        setLoading(true);
        setError(false);
        loadCandies();
    }, []);

    return (
        <div className="product-list">
            {loading && <p>Đang tải dữ liệu ...</p>}
            {error && <p>Không thể tải dữ liệu</p>}

            {candies.map((item) => (
               <div className="product-item">
                    <h1>{ item.name }</h1>
                    <p>$ { item.price }</p>
                </div>
            ))}
        </div>
    )
}

export default CandyList;