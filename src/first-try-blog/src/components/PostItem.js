import React from 'react'

const PostItem = ({id, title, description}) => {
  return (
    <div>
        <h1>
            {title}
        </h1>
        <p>
            {description}
        </p>
    </div>
  )
}

export default PostItem;