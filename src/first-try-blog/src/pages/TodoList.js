import { useState, useEffect, useMemo } from "react";

export default function TodoList() {
    const [todos, setTodos] = useState([]);
    const [searchTerm, setSearchTerm] = useState("");
    const [filterCompleted, setFilterCompleted] = useState("");

    const [currentPage, setCurrentPage] = useState(1);
    const [totalTodos, setTotalTodos] = useState(0);
    const todosPerPage = 10;

    useEffect(() => {
        fetchTodoList();

        async function fetchTodoList() {
            try {
                const res = await fetch(`https://jsonplaceholder.typicode.com/todos`);
                const result = await res.json();
                setTodos(result);
            } catch (error) {
                console.log(error);
            }
        }
    }, []);

    const pageNumbers = [];

    for (let i = 1; i <= Math.ceil(totalTodos / todosPerPage); i++) {
        pageNumbers.push(i);
    }


    const todosData = useMemo(() => {
        let computedTodos = todos || [];

        if (searchTerm) {
            computedTodos = computedTodos.filter(
                todo =>
                    todo.title.toLowerCase().includes(searchTerm.toLowerCase())
            );
        }

        if (filterCompleted === "true") {
            computedTodos = computedTodos.filter(
                todo => todo.completed === true
            )
        }

        if (filterCompleted === "false") {
            computedTodos = computedTodos.filter(
                todo => todo.completed === false
            )
        }

        setTotalTodos(computedTodos.length);

        //Current Page slice
        return computedTodos.slice(
            (currentPage - 1) * todosPerPage,
            (currentPage - 1) * todosPerPage + todosPerPage
        );
    }, [todos, currentPage, searchTerm, filterCompleted]);
    // Change page
    const paginate = (pageNumber) => setCurrentPage(pageNumber);

    const resetFilter = () => {
        setSearchTerm("");
        setFilterCompleted("");
        setCurrentPage(1);
    };

    return (
        <>
            <h3>Filters</h3>

            <div className="mb-3">
                <label htmlFor="search" className="form-label">
                    Search
                </label>
                <input
                    type="text"
                    className="form-control"
                    id="search"
                    placeholder="Search Title"
                    value={searchTerm}
                    onChange={(e) => {
                        setSearchTerm(e.target.value);
                        setCurrentPage(1);
                    }}
                />
            </div>

            <div className="mb-3">
                <label htmlFor="search" className="form-label">
                    Completed
                </label>
                <select
                    className="form-select"
                    value={filterCompleted}
                    onChange={(e) => {
                        setFilterCompleted(e.target.value);
                        setCurrentPage(1);
                    }}
                >
                    <option defaultValue="">All</option>
                    <option value="true">true</option>
                    <option value="false">false</option>
                </select>
            </div>

            <div className="mb-3">
                <button
                    type="button"
                    className="btn btn-danger btn-sm"
                    onClick={resetFilter}
                >
                    Reset Filters
                </button>
            </div>

            <nav>
                <ul className="pagination">
                    {pageNumbers.map((number) => (
                        <li key={number} className="page-item">
                            <button onClick={() => paginate(number)} className="page-link">
                                {number}
                            </button>
                        </li>
                    ))}
                </ul>
            </nav>

            {todosData
                .map((todo) => {
                    return (
                        <div key={todo.id} className="card mb-2">
                            <div className="card-header">
                                <div className="card-header-flex">
                                    <h4 className="id">Todo Item {`#${todo.id}`}</h4>
                                </div>
                            </div>
                            <div className="card-body">
                                <div className="card-text">
                                    <div className="card-body-flex">
                                        <p>{`Title: ${todo.title}`}</p>
                                        <p>{`Completed: ${todo.completed}`}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    );
                })}
        </>
    );
}