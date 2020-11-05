import styled from "styled-components";

const Root = styled.div`
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: stretch;
  width: 500px;
  height: 95vh;
  border: 1px solid #ddd;
  border-radius: 10px;
  @media (max-width: 550px) {
    width: 95%;
  }
`;

const Header = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 24px;
  border-bottom: 1px solid #ddd;
`;

const List = styled.div`
  flex: 1;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  &::-webkit-scrollbar {
    width: 5px;
  }
  &::-webkit-scrollbar-track {
    background: transparent;
  }
  &::-webkit-scrollbar-thumb {
    background: rgba(0, 0, 0, 0.3);
    border-radius: 3px;
  }
  &::-webkit-scrollbar-thumb:hover {
    background: rgba(0, 0, 0, 0.5);
  }
`;

const FabContainer = styled.div`
  position: absolute;
  bottom: 24px;
  right: 24px;
`;

const HeaderButtons = styled.div`
  display: flex;
  align-items: center;
`;

const S = { Root, Header, List, FabContainer, HeaderButtons };

export default S;
