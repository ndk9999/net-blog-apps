import React, { useState } from 'react'

const TermsOfUse = () => {
    const [showTerms, setShowTerms] = useState(false);
    const [numbers, setNumbers] = useState([5, 2, 1, 3 ,4]);

    function showTermsSummaryHandler() {
        setShowTerms(prevState => !prevState);
        setNumbers([...numbers].sort());
    }

    const paragraphText = showTerms ? <p>Lorem, ipsum dolor sit amet consectetur adipisicing elit. Recusandae doloribus unde 
        voluptate quisquam porro eaque molestias voluptatibus! Magni amet debitis saepe cum 
        praesentium voluptate veniam, beatae, ut qui ipsam recusandae quia nulla voluptates 
        adipisci officia quidem repellendus placeat voluptatum alias maxime odit inventore 
        accusantium. Mollitia repellat reiciendis, hic pariatur dicta facere esse totam error. 
        Illum consectetur voluptate fuga ratione tempora quam ducimus consequatur delectus 
        deserunt veniam accusantium quae aperiam porro voluptatibus adipisci, ipsa quasi nesciunt 
        excepturi ex minima, corporis laborum mollitia, esse cupiditate? Repellat vero dolorem 
        accusantium quis recusandae aperiam omnis et delectus vitae. Nostrum fugit maiores 
        minima ducimus repellendus!</p> : null;

    return (
        <section>
            <button onClick={showTermsSummaryHandler}>
                Show Terms Of Use Summary
            </button>
            {paragraphText}
            {showTerms}
            {numbers}
        </section>
    )
}

export default TermsOfUse;