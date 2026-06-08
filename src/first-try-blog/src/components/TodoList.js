import React, { useRef, useState } from 'react'
import './todolist.css'

const TodoList = () => {
    const [items, setItems] = useState(['Finish this book', 'Learn React', 'Do Practice']);
    const todoRef = useRef();

    function addTodoItem() {
        const todo = todoRef.current.value;
        if (todo) {
            setItems(prevState => [todo, ...prevState]);
            todoRef.current.value = '';
        }
    }

    return (
        <div>
            <div>
                <span>Todo</span>
                <input type="text" ref={todoRef} />
                <button onClick={addTodoItem}>Add Todo</button>
            </div>

            <div className='bgRed'>
                {items.map((todo) => (
                    <div key={todo} className='border my-2 p-3'>
                        {todo}
                    </div>
                ))}
            </div>
        </div>
    )
}

export default TodoList;