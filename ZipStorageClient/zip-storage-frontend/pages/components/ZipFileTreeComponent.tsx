import FolderTree, { FolderTreeProps, NodeData } from 'react-folder-tree'
import { Row, Col } from 'react-bootstrap'
import { Trash } from 'grommet-icons/icons'

function FolderTreeComponent({props, deleteCb}: {props: FolderTreeProps, deleteCb:any}) {

    // Add logic around `term`
    return <>
            <div>
                <Col>
                    <Row>
                        <Trash size='medium' onClick={deleteCb} ></Trash>
                    </Row>
                    <Row>
                        <FolderTree {...props}></FolderTree>
                    </Row>
                </Col>
                <br></br>
            </div>
        </>
}

export default FolderTreeComponent
