import React, { useEffect, useState } from 'react'
import { useParams, useSearchParams } from 'react-router-dom';
import PostList from '../components/PostList';
import PostPager from '../components/PostPager';
import PostSearch from '../components/PostSearch';

const Blog = () => {
    const params = useParams();
    const [searchParams, setSearchParams] = useSearchParams();
    console.log('Params: ', params, searchParams.get('page'));

    const [pageNumber, setPageNumber] = useState(1);
    const [keyword, setKeyword] = useState('');
    const [postsList, setPostsList] = useState({
        items: [],
        metadata: {}
    });

    useEffect(() => {
        console.log('Fetching posts ...');
        fetchPostsList(pageNumber, keyword);
    }, [pageNumber, keyword]);

    async function fetchPostsList(page, keyword) {
        const response = await fetch(`http://localhost:5000/api/posts?pageNumber=${page}&pageSize=10&keyword=${keyword}`);
        const result = await response.json();

        setPostsList(result);
    }

    function changeCurrentPageNumber(inc) {
        setPageNumber(prevVal => prevVal + inc);
    }
    
    function updateKeyword(searchTerm) {
        setPageNumber(1);
        setKeyword(searchTerm);
    }

  return (
    <div>
        <h2>
            Newest Posts
        </h2>
        <PostSearch onKeywordChange={updateKeyword} />

        {postsList.items.length ? (
            <>
                <PostList posts={postsList.items} />
                <PostPager 
                    hasNextPage={postsList.metadata.hasNextPage || false}
                    hasPrevPage={postsList.metadata.hasPreviousPage || false}
                    onPageChange={changeCurrentPageNumber}
                />
            </>
        ) : (
            <p className='text-danger'>
                No blog posts found
            </p>
        )}

        
    </div>
  )
}

export default Blog;