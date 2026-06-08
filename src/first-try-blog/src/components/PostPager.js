import React from 'react'

const PostPager = ({hasPrevPage, hasNextPage, onPageChange}) => {
  return (
    <div>
        {hasPrevPage ? (
            <button type='button' className='btn btn-primary' onClick={evt => onPageChange(-1)}>Prev</button>
        ) : (
            <button type='button' className='btn btn-secondary'>Prev</button>
        )}
        {hasNextPage ? (
            <button type='button' className='btn btn-primary' onClick={evt => onPageChange(1)}>Next</button>
        ) : (
            <button type='button' className='btn btn-secondary'>Next</button>
        )}
    </div>
  )
}

export default PostPager;