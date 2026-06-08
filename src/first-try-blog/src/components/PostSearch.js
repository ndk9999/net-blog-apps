import React, { useRef } from 'react'

const PostSearch = ({onKeywordChange}) => {
    const keywordRef = useRef();

    function submitSearchTerm(evt) {
        evt.preventDefault();
        onKeywordChange(keywordRef.current.value);
    }

  return (
    <form onSubmit={submitSearchTerm}>
        Keyword:
        <input type='text' ref={keywordRef} />
        <button type='submit'>Search</button>
        <button type='button' onClick={() => {
            onKeywordChange('');
            keywordRef.current.value = '';
            keywordRef.current.focus();
        }}>Clear</button>
    </form>
  )
}

export default PostSearch;