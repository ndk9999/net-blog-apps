import axios from 'axios';
import React, { useState } from 'react';

const EditCategory = () => {
    const [name, setName] = useState('');
    const [showOnMenu, setShowOnMenu] = useState(false);
    const [message, setMessage] = useState('Vui lòng nhập dữ liệu');

    async function createCategoryHandler(e) {
        e.preventDefault();
        
        try {
            setMessage('Đang gửi dữ liệu ...');

            const res = await axios.post('https://localhost:7076/categories', {
                name, showOnMenu
            });

            setMessage('Đã tạo danh mục thành công');
        } catch (err) {
            setMessage('Đã có lỗi xảy ra');
        }
    }

    return (
        <div>
            <h1>Add New Category</h1>

            {message && (<div style={{color: 'red'}}>{message}</div>)}

            <form onSubmit={createCategoryHandler}>
                <label> Name:
                    <input type="text"
                        onChange={(e) => setName(e.target.value)} />
                </label>
                
                <label> Show On Menu:
                    <input type="checkbox" id="showOnMenu"
                        onChange={(e) => setShowOnMenu(!showOnMenu)} />
                </label>
                
                <button type="submit">Submit</button>
            </form>
        </div>
    )
}

export default EditCategory;