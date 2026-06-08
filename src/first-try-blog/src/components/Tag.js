import React from 'react'

const Tag = (props) => {
    return (
        <li className='border mb-3 p-3'>
            <article>
                <h4 className='text-success'>
                    {props.title} (ID: {props.id})
                </h4>
                <div>
                    {props.children}
                </div>
            </article>
        </li>
    )
}

export default Tag;