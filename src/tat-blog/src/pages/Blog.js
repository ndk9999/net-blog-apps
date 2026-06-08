import React from 'react'
import { useLocation } from 'react-router-dom';
import PostSearch from '../components/blog/posts/PostSearch';

const Blog = () => {
  const queryStrings = new URLSearchParams(useLocation().search);
  const keyword = queryStrings.get('keyword');

  return (
    <div>
      <h1 className='mb-5'>
        {keyword ? (
          <>
            Search results for keyword:
            <span className='text-danger'> {keyword}</span>
          </>
        ) : "Latest Posts"}
      </h1>

      <PostSearch postQuery={{keyword: keyword}} />
    </div>
  )
}

export default Blog;