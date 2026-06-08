import React from 'react'

const TagForm = ({onTagEntered}) => {
  return (
    <div>
        <label htmlFor="tag-name">Enter new tag:</label>
        <input type="text" onChange={onTagEntered} />
    </div>
  )
}

export default TagForm;