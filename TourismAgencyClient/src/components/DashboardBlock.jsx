import React from 'react';
import { Link } from 'react-router-dom';
import './DashboardBlock.css';

/**
 * DashboardBlock component.
 * A reusable block for dashboard navigation.
 *
 * @param {object} props - The component props.
 * @param {string} props.title - The title to display in the block.
 * @param {string} props.description - The description text.
 * @param {string} props.linkTo - The path for the link. If not provided, it renders a non-clickable block.
 * @param {boolean} [props.isEmpty=false] - If true, styles the block as an empty placeholder.
 */
const DashboardBlock = ({ title, description, linkTo, isEmpty = false }) => {
    const content = (
        <>
            <h2>{title}</h2>
            <p>{description}</p>
        </>
    );

    const blockClasses = `dashboard-block ${isEmpty ? 'empty' : ''}`;

    // If a linkTo path is provided, wrap the block in a Link.
    if (linkTo) {
        return (
            <Link to={linkTo} className={blockClasses}>
                {content}
            </Link>
        );
    }

    // Otherwise, render a simple div (useful for the empty block).
    return (
        <div className={blockClasses}>
            {content}
        </div>
    );
};

export default DashboardBlock;
