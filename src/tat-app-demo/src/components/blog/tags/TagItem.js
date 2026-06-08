import React from 'react'

const TagItem = ({slug, tagName, postCount}) => {
  return (
    <a 
        className='btn btn-sm btn-outline-secondary me-2 mb-2'
        href={`/tag/${slug}`}
        title="Click to view posts containing this tag"
    >
        {tagName} {postCount > 0 && `(${postCount})`}
    </a>
  )
}

export default TagItem