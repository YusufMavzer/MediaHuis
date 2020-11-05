import styled from "styled-components";

interface Selected {
  selected?: boolean;
}

const Root = styled.div<Selected>`
  padding: 28px;
  padding-left: 24px;
  padding-right: 24px;
  border-bottom: 1px solid #ddd;
  transition: 0.3s;
  cursor: pointer;
  background-color: ${(props) =>
    props.selected ? "rgba(25, 118, 210, 0.9)" : ""};
  &:hover {
    background-color: ${(props) =>
      props.selected ? "rgba(25, 118, 210, 1)" : "rgba(0, 0, 0, 0.1)"};
  }
`;

const Title = styled.h2<Selected>`
  font-family: "Roboto";
  font-weight: 500;
  font-size: 14pt;
  transition: 0.3s;
  color: ${(props) => (props.selected ? "#FFF" : "#000")};
`;

const Body = styled.p<Selected>`
  font-family: Roboto;
  margin-top: 8px;
  transition: 0.3s;
  color: ${(props) => (props.selected ? "#FFFFFF8a" : "#0000008a")};
`;

const S = { Root, Title, Body };

export default S;
