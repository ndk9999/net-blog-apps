import React from 'react'
import PostItem from './PostItem';

const PostList = ({posts}) => {
  return (
    <div>
        {posts.map(item => (
            <PostItem 
                key={item.id} 
                id={item.id}
                title={item.title}
                description={item.shortDescription} 
            />
        ))}
    </div>
  )
}

export default PostList;